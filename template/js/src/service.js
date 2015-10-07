var ConnectMapService = function ($, mid) {
    var moduleId = mid;
    var baseServicepath = $.dnnSF(moduleId).getServiceRoot('Connect/Map');

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

    this.submitPoint = function(newMapPoint, success) {
        this.ajaxCall('POST', 'MapPoints', 'MapPoint', null, newMapPoint, success);
    }

    this.deletePoint = function(mapPointId, success) {
        this.ajaxCall('POST', 'MapPoints', 'Delete', mapPointId, null, success);
    }

}

module.exports = ConnectMapService;
