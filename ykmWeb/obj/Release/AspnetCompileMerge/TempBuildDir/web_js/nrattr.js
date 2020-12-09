//保存最后的组合结果信息
var SKUResult = {};
//获得对象的key
function getObjKeys(obj) {
    if (obj !== Object(obj)) throw new TypeError('Invalid object');
    var keys = [];
    for (var key in obj)
        if (Object.prototype.hasOwnProperty.call(obj, key))
            keys[keys.length] = key;
    return keys;
}

//把组合的key放入结果集SKUResult
function add2SKUResult(combArrItem, sku) {
    var key = combArrItem.join(";");
    if (SKUResult[key]) {//SKU信息key属性·
        SKUResult[key].count += sku.count;
        SKUResult[key].prices.push(sku.price);
    } else {
        SKUResult[key] = {
            count: sku.count,
            prices: [sku.price]
        };
    }
}

//初始化得到结果集
function initSKU() {
    var i, j, skuKeys = getObjKeys(data);
    for (i = 0; i < skuKeys.length; i++) {
        var skuKey = skuKeys[i];//一条SKU信息key
        var sku = data[skuKey];	//一条SKU信息value
        var skuKeyAttrs = skuKey.split(";"); //SKU信息key属性值数组
        skuKeyAttrs.sort(function (value1, value2) {
            return parseInt(value1) - parseInt(value2);
        });

        //对每个SKU信息key属性值进行拆分组合
        var combArr = combInArray(skuKeyAttrs);
        for (j = 0; j < combArr.length; j++) {
            add2SKUResult(combArr[j], sku);
        }
        //alert(sku.price);
        //结果集接放入SKUResult
        SKUResult[skuKeyAttrs.join(";")] = {
            count: sku.count,
            prices: [sku.price]
        }
    }
}

/**
 * 从数组中生成指定长度的组合
 * 方法: 先生成[0,1...]形式的数组, 然后根据0,1从原数组取元素，得到组合数组
 */
function combInArray(aData) {
    if (!aData || !aData.length) {
        return [];
    }

    var len = aData.length;
    var aResult = [];

    for (var n = 1; n < len; n++) {
        var aaFlags = getCombFlags(len, n);
        while (aaFlags.length) {
            var aFlag = aaFlags.shift();
            var aComb = [];
            for (var i = 0; i < len; i++) {
                aFlag[i] && aComb.push(aData[i]);
            }
            aResult.push(aComb);
        }
    }

    return aResult;
}


/**
 * 得到从 m 元素中取 n 元素的所有组合
 * 结果为[0,1...]形式的数组, 1表示选中，0表示不选
 */
function getCombFlags(m, n) {
    if (!n || n < 1) {
        return [];
    }

    var aResult = [];
    var aFlag = [];
    var bNext = true;
    var i, j, iCnt1;

    for (i = 0; i < m; i++) {
        aFlag[i] = i < n ? 1 : 0;
    }

    aResult.push(aFlag.concat());

    while (bNext) {
        iCnt1 = 0;
        for (i = 0; i < m - 1; i++) {
            if (aFlag[i] == 1 && aFlag[i + 1] == 0) {
                for (j = 0; j < i; j++) {
                    aFlag[j] = j < iCnt1 ? 1 : 0;
                }
                aFlag[i] = 0;
                aFlag[i + 1] = 1;
                var aTmp = aFlag.concat();
                aResult.push(aTmp);
                if (aTmp.slice(-n).join("").indexOf('0') == -1) {
                    bNext = false;
                }
                break;
            }
            aFlag[i] == 1 && iCnt1++;
        }
    }
    return aResult;
}



//初始化用户选择事件
$(function () {
    initSKU();
    //   var endTime = new Date().getTime();
    //   $('#init_time').text('init sku time: ' + (endTime - startTime) + " ms");
    $('.sku').each(function () {
        var self = $(this);
        var attr_id = self.attr('attr_id');


        if (!SKUResult[attr_id]) {
            self.addClass("wu");
        }
    }).click(function () {
        $("#num").val("1");//重置数量
        if ($(this).hasClass("wu")) {
            return false;
        }
        var self = $(this);

        $("#gg_class_" + $(this).data("pid")).val($(this).attr("attr_id"));
        if (!IsPC()) {
            var attrid = "";
            $("input[name='gg_class']").each(function () {
                attrid += $(this).val() + ",";
            });
            if (attrid != "") {
                $("#ggtext").load("/ajaxgetdata/getggname?id=" + escape(attrid) + "&t=" + Math.random());
            }
        }
        //选中自己，兄弟节点取消选中
        self.addClass('sel').siblings().removeClass('sel');

        //已经选择的节点
        var selectedObjs = $('.sel');
        if (selectedObjs.length) {
            //获得组合key价格
            var selectedIds = [];
            selectedObjs.each(function () {
                selectedIds.push($(this).attr('attr_id'));
            });
            selectedIds.sort(function (value1, value2) {
                return parseInt(value1) - parseInt(value2);
            });
            var len = selectedIds.length;
            console.info(selectedIds);
            var prices = SKUResult[selectedIds.join(';')].prices;
            var kcnum = SKUResult[selectedIds.join(';')].count;
            //var maxPrice = Math.max.apply(Math, prices);
            //var minPrice = Math.min.apply(Math, prices);
            //$('#price').text(maxPrice > minPrice ? minPrice + "-" + maxPrice : maxPrice);
            //$('#mprice2').text($('#price').text());
            $('#price').text(parseFloat(prices).toFixed(2));
            $('#kcnum').text("库存：" + kcnum + "件");
            $('#kc').val(kcnum);
            $('#prices').val(parseFloat(prices).toFixed(2));
            $('#showprice').text(parseFloat(prices).toFixed(2));//ykm-2017.9.15页面价格显示
            //用已选中的节点验证待测试节点 underTestObjs
            $(".sku").not(selectedObjs).not(self).each(function () {
                var siblingsSelectedObj = $(this).siblings('.sel');
                var testAttrIds = [];//从选中节点中去掉选中的兄弟节点
                if (siblingsSelectedObj.length) {
                    var siblingsSelectedObjId = siblingsSelectedObj.attr('attr_id');
                    for (var i = 0; i < len; i++) {
                        (selectedIds[i] != siblingsSelectedObjId) && testAttrIds.push(selectedIds[i]);
                    }
                } else {
                    testAttrIds = selectedIds.concat();
                }
                testAttrIds = testAttrIds.concat($(this).attr('attr_id'));
                testAttrIds.sort(function (value1, value2) {
                    return parseInt(value1) - parseInt(value2);
                });
                if (!SKUResult[testAttrIds.join(';')]) {
                    $(this).addClass("wu").removeClass('sel');
                } else {
                    $(this).removeClass('wu');
                }
            });
        } else {
            //设置默认价格
            $('#price').text('--');
            //设置属性状态
            $('.sku').each(function () {
                SKUResult[$(this).attr('attr_id')] ? $(this).removeClass('wu') : $(this).addClass("wu").removeClass('sel');
            })
        }
    });
});

function IsPC() {
    var userAgentInfo = navigator.userAgent;
    var Agents = ["Android", "iPhone",
        "SymbianOS", "Windows Phone",
        "iPad", "iPod"];
    var flag = true;
    for (var v = 0; v < Agents.length; v++) {
        if (userAgentInfo.indexOf(Agents[v]) > 0) {
            flag = false;
            break;
        }
    }
    return flag;
}