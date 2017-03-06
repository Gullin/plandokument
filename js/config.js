// Namespace top level
// written by andrew dupont, optimized by addy osmani
function extend(destination, source) {
    var toString = Object.prototype.toString,
        objTest = toString.call({});
    for (var property in source) {
        if (source[property] && objTest == toString.call(source[property])) {
            destination[property] = destination[property] || {};
            extend(destination[property], source[property]);
        } else {
            destination[property] = source[property];
        }
    }
    return destination;
};

var Lkr = {};


extend(Lkr, {
    Plan: {
        Setting: {
            Map: {
                mapResizeTolerance: 0.1,  // skillnaden mellan nuvarande och ny storlek på kartbehållare (0 - 1)
                mapResizeTimespan: 500    // i millisekunder
            }
        },
        Dokument: {
            resolvedClientUrl: '',
            isPlansSearched: false,
            initExpColAll: false,
            planListInfo: null,
            nbrOfPlanHits: 0,
            nbrOfPlanBoms: 0,
            currentWindowSizeWidth: null,
            currentWindowSizeHeight: null
        },
        AjaxCalls: {
            getPlansDocs: null,
            putMapOfPlan: null
        }
    }
})