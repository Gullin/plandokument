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
                mapResizeTolerance: 0.1,    // skillnaden mellan nuvarande och ny storlek på kartbehållare (0 - 1)
                mapResizeTimespan: 500,      // i millisekunder
                backGroundLayerSettings: {
                    defaultMapByLayer: 'landskrona-bakgrundskarta-color-notext',
                    backgroundtypes: [
                        {
                            title: 'Landskrona',
                            credential: {
                                token: ''
                            },
                            maps: [
                                {
                                    title: 'Landskrona Bakgrundskarta färg textfri',
                                    url: 'https://map-services.landskrona.se/maps/v1/bakgrundskarta-color-notext',
                                    params: {
                                        layers: 'landskrona-bakgrundskarta-color-notext'
                                    }
                                },
                                {
                                    title: 'Landskrona Bakgrundskarta nedtonad textfri',
                                    url: 'https://map-services.landskrona.se/maps/v1/bakgrundskarta-toned-down-notext',
                                    params: {
                                        layers: 'landskrona-bakgrundskarta-toned-down-notext'
                                    }
                                }
                            ]
                        },
                        /*
                        // Externa resurser. Bygger på att reverse proxy fungerar. Per 2025-08-14 gör en implementation i Web.config inte det.
                        {
                            title: 'Lantmäteriet',
                            credential: {
                                basic: {
                                    user: '',
                                    password: ''
                                }
                            },
                            maps: [
                                {
                                    title: 'Topografiska webbkartan färg',
                                    // url: 'https://maps.lantmateriet.se/topowebb/wms/v1?request=GetCapabilities&version=1.1.1',
                                    // url: 'https://maps.lantmateriet.se/topowebb/wms/v1',
                                    url: '/app/plan/proxy/lm-topowebb/topowebb/wms/v1/',
                                    params: {
                                        layers: 'topowebbkartan'
                                    }
                                }
                            ]
                        },
                        {
                            title: 'Trafikverket',
                            credential: {
                            },
                            maps: [
                                {
                                    title: 'Nätinformation',
                                    url: '/app/plan/proxy/trv-netinfo_1_8/MapService/wms.axd/NetInfo_1_8/',
                                    params: {
                                        layers: 'Vagtrafiknat'
                                    },
                                }
                            ]
                        }
                        */
                    ]
                }
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
            putMapOfPlan: null,
            Delay: 500                      // i millisekunder, saktar ner funktion med max motsvarande angiven tid
        }
    }
})