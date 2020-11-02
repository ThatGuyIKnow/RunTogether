export class CommonJS {
    ImagetoPrint(source) {
        return  "<html><head><script>function step1(){\n" +
                "setTimeout('step2()', 10);}\n" +
                "function step2(){window.print();window.close()}\n" +
                "</scri" + "pt></head><body onload='step1()'>\n" +
                "<img src='" + source + "' /></body></html>";
}
    PrintImage(id) {
        let source = document.getElementById(id).src;
        let Pagelink = "about:blank";
        var pwa = window.open(Pagelink, "_new");
        pwa.document.open();
        pwa.document.write(this.ImagetoPrint(source));
        pwa.document.close();
}
}