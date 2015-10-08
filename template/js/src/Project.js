/** @jsx React.DOM */
var <%= props.organization %><%= props.projectName %>Component = require('./<%= props.widgetName %>'),
  <%= props.projectName %>Service = require('./service');

;
(function($, window, document, undefined) {

  $(document).ready(function() {
    <%= props.organization %><%= props.projectName %>.loadData();
  });

  window.<%= props.organization %><%= props.projectName %> = {
    modules: {},

    loadData: function() {
      $('.<%= props.organization %><%= props.projectName %>').each(function(i, el) {
        var moduleId = $(el).data('moduleid');
        var newModule = {
          service: new <%= props.projectName %>Service($, moduleId)
        };
        <%= props.organization %><%= props.projectName %>.modules[moduleId] = newModule;
        <%= props.organization %><%= props.projectName %>.modules[moduleId].service.getInitialData(function(data) {
          <%= props.organization %><%= props.projectName %>.modules[moduleId].settings = data.Settings;
          <%= props.organization %><%= props.projectName %>.modules[moduleId].<%= props.widgetName %>s = data.<%= props.widgetName %>s;
          <%= props.organization %><%= props.projectName %>.modules[moduleId].security = data.Security;
          <%= props.organization %><%= props.projectName %>.modules[moduleId].resources = data.ClientResources;
        });
      });
    }

  }


})(jQuery, window, document);
