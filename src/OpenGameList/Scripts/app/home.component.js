System.register(["@angular/core"], function (exports_1, context_1) {
    "use strict";
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __moduleName = context_1 && context_1.id;
    var core_1, HomeComponent;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            }
        ],
        execute: function () {
            HomeComponent = (function () {
                function HomeComponent() {
                    this.title = "Welcome VIew";
                }
                return HomeComponent;
            }());
            HomeComponent = __decorate([
                core_1.Component({
                    selector: "opengamelist",
                    template: " \n        <h1>{{title}}</h1>\n\n        <item-list class=\"latest\"></item-list>\n\n        <item-list class=\"most-viewed\"></item-list>\n\n        <item-list class=\"random\"></item-list>\n\n    ",
                    styles: ["\n\n\n        item-list {\n\n            min-width: 332px;\n\n            border: 1px solid #aaaaaa;\n\n            display: inline-block;\n\n            margin: 0 10px;\n\n            padding: 10px;\n\n        }\n\n        item-list.latest {\n\n            background-color: #f9f9f9;\n\n        }\n\n        item-list.most-viewed {\n\n            background-color: #f0f0f0;\n\n        }\n\n        item-list.random {\n\n            background-color: #e9e9e9;\n\n        }\n\n    "]
                })
            ], HomeComponent);
            exports_1("HomeComponent", HomeComponent);
        }
    };
});
