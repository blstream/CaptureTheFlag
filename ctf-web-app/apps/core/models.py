import logging
from django.utils.translation import ugettext_lazy as _
from django.contrib.auth.models import AbstractUser
from django.db import models, transaction
from apps.core.constants import CHARACTER_TYPES, DEVICE_TYPES, TEAM_TYPES

logger = logging.getLogger('root')


class Location(object):
    """ Represents location object with latitude and longitude. It's using by LocationField object.
    """

    def __init__(self, lat=0.00, lon=0.00):
        self.lat = lat
        self.lon = lon

    def __repr__(self):
        return str(self.lat) + ', ' + str(self.lon)


class LocationField(models.Field):
    """ Custom location field which stores data in db as a string separated by comma, e.g. <latitude>, <longitude>.
    """
    description = "Location field witch stores latitude and longitude"

    __metaclass__ = models.SubfieldBase

    def __init__(self, help_text="A comma-separated latitude longitude pair", *args, **kwargs):
        self.name = "LocationField",
        self.through = None
        self.help_text = help_text
        self.blank = True
        self.editable = True
        self.creates_table = False
        self.db_column = None
        self.serialize = False
        self.null = True
        self.creation_counter = models.Field.creation_counter
        kwargs["max_length"] = 100
        models.Field.creation_counter += 1
        super(LocationField, self).__init__(*args, **kwargs)

    def db_type(self, connection):
        return 'varchar(100)'

    def to_python(self, value):
        if value in (None, ''):
            return Location()
        else:
            if isinstance(value, Location):
                return value
            else:
                args = [float(value.split(',')[0]), float(value.split(',')[1])]
                if len(args) != 2 and value is not None:
                    raise models.exceptions.ValidationError("Invalid input for a Location instance")
                return Location(*args)

    def get_prep_value(self, value):
        logger.debug("get_prep_value: value: %s (%s)", value, type(value))

        if value and isinstance(value, Location):
            return ', '.join([str(value.lat), str(value.lon)])
        return value

    def value_to_string(self, obj):
        value = self._get_val_from_obj(obj)
        return self.get_prep_value(value)


class GeoModelManager(models.Manager):
    """ GeoModel manager class.
    """


class GeoModel(models.Model):
    """ GeoModel object.
    """
    location = LocationField(null=True, blank=True, verbose_name=_("Location"))

    def get_location(self):
        logger.debug("location: %s (%s)", self.location, type(self.location))
        return "%s, %s" % (self.location.lon, self.location.lat)

    class Meta:
        abstract = True


class Character(models.Model):
    user = models.ForeignKey('PortalUser', related_name="characters")
    type = models.IntegerField(blank=False, choices=CHARACTER_TYPES, verbose_name=_("Type"))
    total_time = models.IntegerField(blank=False, default=0, verbose_name=_("Total time"))
    total_score = models.IntegerField(blank=False, default=0, verbose_name=_("Total score"))
    health = models.DecimalField(blank=False, max_digits=3, default=1.00, decimal_places=2, verbose_name=_("Health"))
    level = models.IntegerField(blank=False, default=0, verbose_name=_("Level"))

    def __unicode__(self):
        return "%s: %s" % (self.type, self.user)

    class Meta:
        app_label = "core"


class PortalUser(GeoModel, AbstractUser):
    nick = models.CharField(blank=False, max_length=100, verbose_name=_("Nick"))

    device_type = models.IntegerField(blank=True, null=True, choices=DEVICE_TYPES, verbose_name=_("Device type"))
    device_id = models.CharField(blank=True, null=True, max_length=255, verbose_name=_("Device ID"))

    team = models.IntegerField(blank=True, null=True, choices=TEAM_TYPES, verbose_name=_("Team"))
    current_game_id = models.IntegerField(blank=True, null=True, verbose_name=_("Current game id"))

    # todo in v2.0: characters
    # active_character = models.ForeignKey(Character, null=True, blank=True, verbose_name=_("Active character"))

    AbstractUser._meta.get_field("email").blank = False
    AbstractUser._meta.get_field("email").null = False

    @transaction.atomic
    def save(self, *args, **kwargs):
        super(PortalUser, self).save(*args, **kwargs)

        # todo in v2.0: characters
        # characters = self.characters.all()
        # if not characters:
        #     for character_type in CHARACTER_TYPES:
        #         character = Character(user=self, type=character_type[0])
        #         character.save()
        #         if character.type == 0:
        #             self.active_character = character
        #     logger.info("characters were saved for user: %s - count: %d", self.username, len(characters))
        # else:
        #     logger.debug("characters already exist in user: %s - count: %d", self.username, len(characters))

    @classmethod
    def get_device_type(cls, device_type):
        """ Returns device type based on string value.
        """
        try:
            logger.debug("get_device_type by value: %s (type: %s)", device_type, type(device_type))
            device_type = int(device_type)
            return DEVICE_TYPES[device_type][0]
        except ValueError:
            return None
        except IndexError:
            return None

    def __unicode__(self):
        return "%s (team: %s)" % (self.username, self.team)

    class Meta:
        app_label = "core"
        swappable = 'AUTH_USER_MODEL'
