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

    WriteCookie(name, value, days) {
        let expires;
        if (days) {
            let date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        }
        else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    }

    ReadCookie(name) {
        let nameEQ = name + "=";
        let ca = document.cookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    CopyToClipboard(str) {
        const textarea = document.createElement("textarea");
        textarea.value = str;
        textarea.style.top = "0";
        textarea.style.left = "0";
        textarea.style.position = "fixed";

        document.body.appendChild(textarea);

        textarea.select();
        textarea.setSelectionRange(0, 99999); /*For mobile devices*/

        document.execCommand("copy");
        document.body.removeChild(textarea);
    }
}