from django.conf.urls import patterns, include, url

from django.contrib import admin

admin.autodiscover()

urlpatterns = patterns(
    '',
    url(r'^admin/', include(admin.site.urls)),

    url(r'^', include('apps.core.urls')),

    url(r'^oauth2/', include('provider.oauth2.urls', namespace='oauth2')),
    url(r'^api/', include('apps.core.api.urls', namespace='api_core')),
)
