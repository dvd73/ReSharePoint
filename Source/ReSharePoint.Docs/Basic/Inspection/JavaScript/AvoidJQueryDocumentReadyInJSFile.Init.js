﻿window.myappName$ = window.myappName$ || jQuery.noConflict(true);

(function ($) {
    if (!_spBodyOnLoadCalled) {
        _spBodyOnLoadFunctions.push(pageLoad);
    } else {
        pageLoad();
    }

    function pageLoad() {
        $('#pageTitle span').text('Hello from jQuery');
    }

    RegisterModuleInit(_spPageContextInfo.webServerRelativeUrl + 'appname/script.js', pageLoad);

})(window.myappName$);