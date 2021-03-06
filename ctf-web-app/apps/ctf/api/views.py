import logging
from django.http import Http404
from rest_framework import status
from rest_framework.generics import get_object_or_404
from rest_framework.mixins import CreateModelMixin, UpdateModelMixin, DestroyModelMixin
from rest_framework.response import Response
from rest_framework.views import APIView
from rest_framework.viewsets import GenericViewSet
from apps.core.api import mixins
from apps.core.api.serializers import GeoModelSerializer
from apps.core.exceptions import AlreadyExistException, GameAlreadyStartedException, GameAlreadyFinishedException, \
    GameNotStartedException
from apps.core.models import PortalUser, Location
from apps.ctf.api.serializers.common import ItemSerializer
from apps.ctf.api.serializers.games import GameSerializer, MarkerSerializer
from apps.ctf.models import Game, Item

__author__ = 'mkr'

logger = logging.getLogger("root")


class GameViewSet(mixins.ModelPermissionsMixin,
                  CreateModelMixin,
                  UpdateModelMixin,
                  DestroyModelMixin,
                  mixins.RetrieveModelMixin,
                  mixins.ListModelMixin,
                  GenericViewSet):
    serializer_class = GameSerializer
    model = Game

    def pre_save(self, obj):
        user = self.request.user
        setattr(obj, "owner", user)


class ItemViewSet(mixins.ModelPermissionsMixin,
                  CreateModelMixin,
                  UpdateModelMixin,
                  DestroyModelMixin,
                  mixins.RetrieveModelMixin,
                  mixins.ListModelMixin,
                  GenericViewSet):
    serializer_class = ItemSerializer
    model = Item


class InGameLocation(APIView):
    def post(self, request, pk, format=None):
        user = request.user
        try:
            logger.debug("looking for a game with id: '%s'", pk)
            game = user.joined_games.get(id=pk)
        except Game.DoesNotExist, e:
            logger.error(e)
            raise Http404
        else:
            serializer = GeoModelSerializer(data=request.DATA)
            if serializer.is_valid():
                user = request.user
                lat = serializer.object.get('lat')
                lon = serializer.object.get('lon')
                user.location = Location(lat, lon)
                user.save()

                logger.debug("location: %s", user.location)

                context = {'request': request}

                markers = game.get_markers(user, context)

                logger.debug("markers size: %d", len(markers))
                logger.debug("markers: %s", markers)

                serializer = MarkerSerializer(markers, context=context, many=True)
                data = serializer.data
                logger.debug("data: %s type(%s)", data, type(data))

                json_data = {
                    "markers": data,
                    "game": {
                        "red_team_points": game.red_team_points,
                        "blue_team_points": game.blue_team_points,
                        "time_to_end": game.get_time_to_end(),
                        "status": game.status
                    }
                }

                return Response(data=json_data, status=status.HTTP_200_OK)

            return Response(serializer.errors, status.HTTP_400_BAD_REQUEST)


class JoinToGame(APIView):
    def post(self, request, pk):
        logger.debug("joining player to selected game...")

        game = get_object_or_404(Game, pk=pk)
        user = request.user

        try:
            game.add_player(user)
        except AlreadyExistException, e:
            return Response(data={"error": e.message}, status=status.HTTP_400_BAD_REQUEST)
        except AssertionError, e:
            # todo: add error code
            return Response(data={"error": e.message}, status=status.HTTP_400_BAD_REQUEST)
        except Exception, e:
            # todo: add error code
            return Response(data={"error": e.message}, status=status.HTTP_400_BAD_REQUEST)
        else:
            logger.info("Player '%s' was added into the game '%s'", user.username, game.name)
            return Response(status=status.HTTP_200_OK)

    def delete(self, request, pk):
        logger.debug("removing player from selected game...")

        game = get_object_or_404(Game, pk=pk)
        user = request.user

        try:
            game.remove_player(user)
        except PortalUser.DoesNotExist, e:
            return Response(data={"error": e.message}, status=status.HTTP_404_NOT_FOUND)
        except AssertionError, e:
            return Response(data={"error": e.message}, status=status.HTTP_400_BAD_REQUEST)
        else:
            logger.info("Player '%s' is no longer in game '%s'", user.username, game.name)
            return Response(status=status.HTTP_200_OK)


class StartGame(APIView):
    def post(self, request, pk):
        logger.debug("starting selected game...")

        game = get_object_or_404(Game, pk=pk)
        user = request.user

        try:
            game.start()
        except GameAlreadyStartedException:
            logger.error("Game: '%s' already started", game)
            return Response(data={"error": "Game already started"}, status=status.HTTP_400_BAD_REQUEST)
        except GameAlreadyFinishedException:
            logger.error("Game: '%s' already finished", game)
            return Response(data={"error": "Game already finish"}, status=status.HTTP_400_BAD_REQUEST)
        except Exception, e:
            # todo: add error code
            return Response(data={"error": e.message}, status=status.HTTP_400_BAD_REQUEST)
        else:
            logger.info("Player '%s' was started the game '%s'", user.username, game.name)
            return Response(status=status.HTTP_200_OK)


class StopGame(APIView):
    def post(self, request, pk):
        logger.debug("stopping selected game...")

        game = get_object_or_404(Game, pk=pk)
        user = request.user

        try:
            game.stop()
        except GameAlreadyFinishedException:
            logger.error("Game: '%s' already finished", game)
            return Response(data={"error": "Game already finished"}, status=status.HTTP_400_BAD_REQUEST)
        except GameNotStartedException:
            logger.error("Game: '%s' not started", game)
            return Response(data={"error": "Game not started"}, status=status.HTTP_400_BAD_REQUEST)
        except Exception, e:
            # todo: add error code
            return Response(data={"error": e.message}, status=status.HTTP_400_BAD_REQUEST)
        else:
            logger.info("Player '%s' was stopped the game '%s'", user.username, game.name)
            return Response(status=status.HTTP_200_OK)