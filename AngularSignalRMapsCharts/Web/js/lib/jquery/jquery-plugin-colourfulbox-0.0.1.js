(function ($) {
    // Change text color of element to something random.
    $.fn.colourfy = function (options) {

        debug(this);

        // Override plugin default settings with options
        var settings = $.extend({}, $.fn.colourfy.defaultSettings, options);

        $(this).on('click', function () {
        
            try {
                var red = parseInt(Math.random() * 255);
                var green = parseInt(Math.random() * 255);
                var blue = parseInt(Math.random() * 255);

                $(this).css({
                    'color': 'rgb(' + red + ',' + green + ',' + blue + ')',
                    'font-size': settings.size    
                });

                $(this).html($.fn.colourfy.format($(this).html()));
            }
            catch (e)
            { }
        });

        done();

        return this;
    };

    // Plugin default settings
    $.fn.colourfy.defaultSettings = {

        //callback functions
        done: function () { },

        //properties
        size: '1em'
    };

    // public function that can be overriden
    $.fn.colourfy.format = function (txt) {
        return "<strong>" + txt + "</strong>";
    };

    // Private function for debugging.
    function debug($obj) {
        if (window.console && window.console.log) {
            window.console.log("colourfy selection count: " + $obj.size());
        }
    };

    function done() {
        $.fn.colourfy.defaultSettings.done.call(this);
    }
}(jQuery));

// Example of modifying the default settings
$.fn.colourfy.defaultSettings.size = "2em";
// Example of overriding public function
//$.fn.colourfy.format = function (txt) { return "<small>" + txt + "</small>"; };
