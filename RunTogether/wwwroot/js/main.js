var Main;Main=(()=>{"use strict";var e={579:(e,t,o)=>{function r(){console.log("Funktion som tester om der er forbindelse")}let a;function n(){a=L.map("mapid").setView([57.0117789,9.9907118],6),L.tileLayer("https://stamen-tiles-{s}.a.ssl.fastly.net/watercolor/{z}/{x}/{y}.{ext}",{attribution:'Map tiles by <a href="http://stamen.com">Stamen Design</a>, <a href="http://creativecommons.org/licenses/by/3.0">CC BY 3.0</a> &mdash; Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',minZoom:6,maxZoom:16,ext:"jpg"}).addTo(a),L.marker([57.0117789,9.9907118]).bindPopup('Start for segment 1<br/>Dette segment er sponseret af State.</p><br/><img src="/logos/State_Logo_v1.jpg" asp-append-version="true" width="300px" />').openPopup().addTo(a),L.marker([57.00967,10.00404]).bindPopup('Start for segment 2<br />Dette segment er sponseret af FrugtKurven.</p><br/><img src="/logos/Frugtkurven_Logo.png" asp-append-version="true" width="300px" />').openPopup().addTo(a),L.polyline([[57.0117789,9.9907118],[57.00967,10.00404]],{color:"#db5c57"}).addTo(a),console.log("leaflet_start() function is done ")}function s(){a.locate({setView:!0})}function p(){L.polyline([[57.0117789,9.9907118],[57.0123239,9.9939051],[57.0123239,9.9939051]],{color:"red"}).addTo(a),console.log("To marker? tak")}o.r(t),o.d(t,{test:()=>r,leaflet_start:()=>n,locateUser:()=>s,addMarker:()=>p})}},t={};function o(r){if(t[r])return t[r].exports;var a=t[r]={exports:{}};return e[r](a,a.exports,o),a.exports}return o.d=(e,t)=>{for(var r in t)o.o(t,r)&&!o.o(e,r)&&Object.defineProperty(e,r,{enumerable:!0,get:t[r]})},o.o=(e,t)=>Object.prototype.hasOwnProperty.call(e,t),o.r=e=>{"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})},o(579)})();