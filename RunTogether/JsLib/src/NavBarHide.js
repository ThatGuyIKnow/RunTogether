export class NavigationalBarHideFunctionality
{
    /* Close the sidebar */
    navbarToggleOpen(style) {
        document.getElementById(style.id).style[style.attribute] = style.value;
    }

    /* Open the sidebar */
    navbarToggleClose(style) {
        document.getElementById(style.id).style[style.attribute] = style.value;
    }
}