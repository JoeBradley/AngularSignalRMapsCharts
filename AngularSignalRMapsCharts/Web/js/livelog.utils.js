String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
          ? args[number]
          : match
        ;
    });
};

Date.prototype.timeAgo = function () {
    //console.log("set Timeago");

    var span = {};
    span.Time = Math.abs((new Date()).getTime() - this.getTime()),
    span.Seconds = Math.ceil(span.Time / 1000);
    span.Minutes = Math.floor(span.Seconds / 60);
    span.Hours = Math.floor(span.Minutes / 60);
    span.Days = Math.floor(span.Hours / 24);
    span.Weeks = Math.floor(span.Days / 7);
    span.Months = Math.floor(span.Days / 30);
    span.Years = Math.floor(span.Days / 365);

    //console.log("Timeago: " + JSON.stringify(span, null, '\t'));

    if (span.Years > 0)
        return "about {0} {1} ago".format(years, years == 1 ? "year" : "years");    
    if (span.Months > 0)
        return "about {0} {1} ago".format(span.Months, span.Months == 1 ? "month" : "months");
    if (span.Weeks > 0)
        return "about {0} {1} ago".format(span.Weeks, span.Weeks == 1 ? "week" : "weeks");
    if (span.Days > 0)
        return "about {0} {1} ago".format(span.Days, span.Days == 1 ? "day" : "days");
    if (span.Hours > 0)
        return "about {0} {1} ago".format(span.Hours, span.Hours == 1 ? "hour" : "hours");
    if (span.Minutes > 0)
        return "about {0} {1} ago".format(span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
    if (span.Seconds > 3)
        return "about {0} seconds ago".format(span.Seconds);

    return "just now";
};