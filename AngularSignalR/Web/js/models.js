// see: http://documentcloud.github.io/backbone/docs/todos.html
// see: http://backbonejs.org/#Model
var Product = Backbone.Model.extend({
    
    idAttribute: 'Id',

    defaults: {
        Price: 0.00
    },

    like: function () {
        var count = this.get('Likes') + 1;
        this.set({ Likes: count });
        this.save();
        console.log('update likes:' + JSON.stringify(this.attributes));
        return this.get("Likes");
    }

});

var Category = Backbone.Model.extend({
    defaults: {
        id: 0,
        title: ""
    },

    initialize: function() {
        this.set('Products', new Products);
    }
});

var Store = Backbone.Model.extend({
    defaults: {
        id: 0,
        Title: "",
    },

    initialize: function () {
        this.set('Products', new Products);
    }
});