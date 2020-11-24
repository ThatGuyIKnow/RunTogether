export class SidebarCollapseFunctionality {
    /* Open or close the sidebar */
    sidebarToggle(style) {

        /* Gets object like so:
         * { id: "myElement", attribute: "width", attributeAlt: "display", value: "15rem", valueAlt: "" }
         */
        document.getElementById(style.id).style[style.attribute] = style.value;
    }
}