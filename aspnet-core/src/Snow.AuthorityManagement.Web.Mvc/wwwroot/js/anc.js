var singleton = function(fn) {
    var result;
    return function() {
        return result || (result = fn.apply(this, arguments));
    };
};

var createMask = singleton(function () {
    var anc = {
        auth: new Array()
    };
    return anc;

});