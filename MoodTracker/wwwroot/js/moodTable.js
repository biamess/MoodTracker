// Handler for mouse moving into of mood in mood table key.
// Hide mood cells in the mood table that don't match the mood being hovered over.
function onMoodHover(event) {
    let t = event.target;

    while (t && !t.id) {
        t = t.parentNode;
    }

    if (t) {
        let moodId = t.id;

        let cells = document.getElementsByClassName("filterable");

        for (let i = 0; i < cells.length; i++) {
            if (!cells[i].classList.contains("mood" + moodId)) {
                cells[i].classList.add("hide");
            }
        }
    }
}

// Handler for mouse moving out of mood in mood table key
// Un-hide any hidden table cells.
function onMoodHoverOut() {
    let cells = document.getElementsByClassName("filterable");

    for (let i = 0; i < cells.length; i++) {
        cells[i].classList.remove("hide");
    }
}