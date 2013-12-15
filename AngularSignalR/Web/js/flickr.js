var flickr = {
    version: '1.0.0',
    author: 'Christopher Cassidy',
    apiKey: 'e9adbe3b99b654bb803545f38ce6131f',
    apiEndpointUrl: 'http://api.flickr.com/services/rest/',

    getImage: function (name, category) {
        var method = 'flickr.photos.search';

        var url = this.apiEndpointUrl + '?' +
            'method=' + method +
            '&api_key=' + this.apiKey +
            '&tags=' + encodeURI(name + ',' + category) +
            '&tag_mode=all&sort=relevance&safe_search=1&content_type=1&per_page=20&page=1&format=json&nojsoncallback=1';

        //console.log('Flickr Search: ' + url);

        try {
            var json = $.ajax({
                url: url,
                async: false,
                dataType: 'json',
                type: "GET"
            }).responseText;

            //console.log(json);
            var photos = $.parseJSON(json).photos;
            var photo = photos.photo[parseInt(Math.random() * photos.photo.length)];

            return 'http://farm' + photo.farm + '.static.flickr.com/' + photo.server + '/' + photo.id + '_' + photo.secret + '_q.jpg';
        }
        catch (e) {
            console.error(e.message);
            return '/web/images/products/missing-image.jpg';
        }
    },
}