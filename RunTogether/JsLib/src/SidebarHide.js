﻿export class SidebarCollapseFunctionality
{
    /* Open or close the sidebar */
    sidebarToggle(style) {

        /* Gets object like so:
         * { id: "myElement", attribute: "width", value: "15rem" }
         */
        document.getElementById(style.id).style[style.attribute] = style.value;
    }
}