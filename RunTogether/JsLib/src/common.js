export class CommonJS {
    ImagetoPrint(source, text) {
        return "<html><head><script>function step1(){setTimeout('step2()', 10);}function step2(){window.print();window.close()}</script><style>@media print {  @page { margin: 0; }  body { margin: 1.6cm; }}html, body, #wrapper {   height:100%;   width: 100%;   margin: 0;   padding: 0;   border: 0;}#wrapper td {   vertical-align: middle;   text-align: center;}</style></head><body onload='step1()'><table id='wrapper'><tr><td><img src='" + source + "'/><h1>" + text + "</h1></td>  </tr></table></body></html>"

    }
    PrintImage(id, text) {
        let source = document.getElementById(id).src;
        let Pagelink = "about:blank";
        var pwa = window.open(Pagelink, "_new");
        pwa.document.open();
        pwa.document.write(this.ImagetoPrint(source, text));
        pwa.document.close();
}
}