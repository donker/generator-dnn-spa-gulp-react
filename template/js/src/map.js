/** @jsx React.DOM */
var ConnectMapComponent = require('./ConnectMap'),
  MapService = require('./service');

;
(function($, window, document, undefined) {

  $(document).ready(function() {
    if ($('#connectMapPanel').length == 0) {
      $('body').append('<div id="connectMapPanel" class="connectMapPanel"></div>');
    };
    ConnectMap.loadData();
  });

  window.ConnectMap = {
    modules: {},
    googleLoaded: false,
    nrMapsToLoad: 0,

    loadData: function() {
      nrMapsToLoad = $('.connectMap').length;
      $('.connectMap').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var newModule = {
          service: new MapService($, moduleId)
        };
        ConnectMap.modules[moduleId] = newModule;
        ConnectMap.modules[moduleId].service.getInitialData(function(data) {
          ConnectMap.modules[moduleId].settings = data.Settings;
          ConnectMap.modules[moduleId].mapPoints = data.MapPoints;
          ConnectMap.modules[moduleId].security = data.Security;
          ConnectMap.modules[moduleId].resources = data.ClientResources;
          if (nrMapsToLoad == 1) {
            ConnectMap.loadGoogle(ConnectMap.modules[moduleId].settings.GoogleMapApiKey);
          }
          nrMapsToLoad -= 1;
        });
      });
    },

    loadMaps: function() {
      $('.connectMap').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        React.render(<ConnectMapComponent moduleId={moduleId} />, el);
      });
    },

    loadGoogle: function(apiKey) {
      if (ConnectMap.googleLoaded) {
        return;
      }
      var googleScript = 'http://maps.googleapis.com/maps/api/js?callback=ConnectMap.loadMaps';
      if (apiKey && apiKey !== '') {
        googleScript += '&key=' + $('.connectMap').first().data('googlekey');
      }
      ConnectMap.loadScript(googleScript);
    },

    slidePanel: function(panel) {
      if (panel.css('display') == 'block') {
        $('body').off("click");
        panel.animate({
          right: -window.innerWidth
        }, 800, function() {
          panel.css('display', 'none');
          $('body').css('overflow', 'auto');
        });
      } else {
        $('body').css('overflow', 'hidden');
        panel.css('right', -window.innerWidth);
        panel.css('display', 'block');
        panel.css('top', $(document).scrollTop());
        var that = this;
        panel.animate({
          right: 0
        }, 800, function() {
          var elementsToFocus = document.getElementsByClassName("formFocus");
          if (elementsToFocus.length > 0) {
            elementsToFocus[0].focus();
          }
          $('body').on("click", function(e) {
            if ($(e.target).closest('#' + panel[0].id).length == 0) {
              that.slidePanel(panel);
            }
          });
        });
      }
    },

    formatString: function(format) {
      var args = Array.prototype.slice.call(arguments, 1);
      return format.replace(/{(\d+)}/g, function(match, number) {
        return typeof args[number] != 'undefined' ? args[number] : match;
      });
    },

    loadScript: function(src, callback) {
      var s,
        r,
        t;
      r = false;
      s = document.createElement('script');
      s.type = 'text/javascript';
      s.src = src;
      s.onload = s.onreadystatechange = function() {
        if (!r && (!this.readyState || this.readyState == 'complete')) {
          r = true;
          if (callback !== undefined) {
            callback();
          }
        }
      };
      t = document.getElementsByTagName('script')[0];
      t.parentNode.insertBefore(s, t);
    }
  }


})(jQuery, window, document);
