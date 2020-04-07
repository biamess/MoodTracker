// Handler for mouse moving into of mood in mood table key.
function onMoodHover(moodId) {
    filterMoodCells(moodId);
}

// Handler for mouse moving out of mood in mood table key
function onMoodHoverOut() {
    unfilterMoodCells();
}

// Hide mood cells in the mood table that don't match the mood with moodId.
function filterMoodCells(moodId) {
    let cells = getFilterableMoodCells();
    for (let i = 0; i < cells.length; i++) {
        if (!cells[i].classList.contains("mood" + moodId)) {
            hideMoodCell(cells[i]);
        }
    }
}

// Un-hide any hidden mood cells.
function unfilterMoodCells() {
    let cells = getFilterableMoodCells();
    for (let i = 0; i < cells.length; i++) {
        unhideMoodCell(cells[i]);
    }
}

function getFilterableMoodCells() {
    return document.getElementsByClassName("filterable");
}

function hideMoodCell(cell) {
    cell.classList.add("hide");
}

function unhideMoodCell(cell) {
    cell.classList.remove("hide");
}