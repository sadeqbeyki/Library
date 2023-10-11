// save selected list
var selectedAuthors = [];
var selectedPublishers = [];
var selectedTranslators = [];

function initializeSelectionLists() {
    selectedAuthors = [];
    selectedPublishers = [];
    selectedTranslators = [];
}

// Is Repeated ??
function isDuplicate(itemName, selectedItemList) {
    return selectedItemList.includes(itemName);
}

function addToSelectedList(inputId, listId, errorId, selectedItemList) {
    var input = document.getElementById(inputId);
    var inputValue = input.value.trim();

    if (inputValue !== "") {
        if (!isDuplicate(inputValue, selectedItemList)) {
            var selectedList = document.getElementById(listId);
            var listItem = document.createElement("li");
            listItem.className = "list-group-item";
            listItem.textContent = inputValue;

            // Add deleted button to items
            var removeButton = document.createElement("button");
            removeButton.className = "btn btn-danger btn-sm float-left";
            removeButton.textContent = "Delete";
            removeButton.onclick = function () {
                selectedItemList = selectedItemList.filter(function (item) {
                    return item !== inputValue;
                });
                selectedList.removeChild(listItem);
                updateHiddenFields(); 
            };
            listItem.appendChild(removeButton);

            selectedItemList.push(inputValue);
            selectedList.appendChild(listItem);
            input.value = ""; 
            document.getElementById(errorId).textContent = ""; 
            updateHiddenFields(); 
        } else {
            document.getElementById(errorId).textContent = "duplicate";
        }
    }
}

function updateHiddenFields() {
    var SelectedAuthors = selectedAuthors.join(",");
    var SelectedPublishers = selectedPublishers.join(",");
    var SelectedTranslators = selectedTranslators.join(",");

    console.log("SelectedAuthors: " + SelectedAuthors);
    console.log("SelectedPublishers: " + SelectedPublishers);
    console.log("SelectedTranslators: " + SelectedTranslators);

    document.getElementById("SelectedAuthors").value = SelectedAuthors;
    document.getElementById("SelectedPublishers").value = SelectedPublishers;
    document.getElementById("SelectedTranslators").value = SelectedTranslators;
}


// add selected author
document.getElementById("addAuthorButton").addEventListener("click", function () {
    addToSelectedList("authorInput", "selectedAuthorsList", "duplicateAuthorError", selectedAuthors);
});
// add selected Translator
document.getElementById("addTranslatorButton").addEventListener("click", function () {
    addToSelectedList("translatorInput", "selectedTranslatorsList", "duplicateTranslatorError", selectedTranslators);
});

// add selected Publisher
document.getElementById("addPublisherButton").addEventListener("click", function () {
    addToSelectedList("publisherInput", "selectedPublishersList", "duplicatePublisherError", selectedPublishers);
});