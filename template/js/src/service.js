var <%= props.organization %><%= props.projectName %>Service = function ($, mid) {
    var moduleId = mid;
    var baseServicepath = $.dnnSF(moduleId).getServiceRoot('<%= props.organization %>/<%= props.projectName %>');

    this.ajaxCall = function(type, controller, action, id, data, success, fail) {
        // showLoading();
        $.ajax({
            type: type,
            url: baseServicepath + controller + '/' + action + (id != null ? '/' + id : ''),
            beforeSend: $.dnnSF(moduleId).setModuleHeaders,
            data: data
        }).done(function(retdata) {
            // hideLoading();
            if (success != undefined) {
                success(retdata);
            }
        }).fail(function(xhr, status) {
            // showError(xhr.responseText);
            if (fail != undefined) {
                fail(xhr.responseText);
            }
        });
    }

    this.getInitialData = function(success) {
        this.ajaxCall('GET', 'Module', 'InitialData', null, null, success);
    }

    this.updateSettings = function(newSettings, success) {
        this.ajaxCall('POST', 'Settings', 'Update', null, newSettings, success);
    }

    this.submitPoint = function(new<%= props.widgetName %>, success) {
        this.ajaxCall('POST', '<%= props.widgetName %>s', '<%= props.widgetName %>', null, new<%= props.widgetName %>, success);
    }

    this.deletePoint = function(id, success) {
        this.ajaxCall('POST', '<%= props.widgetName %>s', 'Delete', id, null, success);
    }

}

module.exports = <%= props.organization %><%= props.projectName %>Service;
