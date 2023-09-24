// save selected list
var selectedAuthors = [];
var selectedTranslators = [];
var selectedPublishers = [];

function initializeSelectionLists() {
    selectedAuthors = [];
    selectedTranslators = [];
    selectedPublishers = [];
}

// Is Repeated ??
function isDuplicate(itemName, selectedItemList) {
    return selectedItemList.includes(itemName);
}

// // add to selectedItemList
// function addToSelectedList(inputId, listId, errorId, selectedItemList {
//     var input = document.getElementById(inputId);
//     var inputValue = input.value.trim();

//     if (inputValue !== "") {
//         if (!isDuplicate(inputValue, selectedItemList)) {
//             var selectedList = document.getElementById(listId);
//             var listItem = document.createElement("li");
//             listItem.className = "list-group-item";
//             listItem.textContent = inputValue;

//             // Add deleted button to items
//             var removeButton = document.createElement("button");
//             removeButton.className = "btn btn-danger btn-sm float-left";
//             removeButton.textContent = "Delete";
//             removeButton.onclick = function () {
//                 selectedItemList = selectedItemList.filter(function (item) {
//                     return item !== inputValue;
//                 });
//                 selectedList.removeChild(listItem);
//             };
//             listItem.appendChild(removeButton);


//             selectedItemList.push(inputValue); // add item to selectedItemList
//             selectedList.appendChild(listItem);
//             input.value = ""; // clear input
//             document.getElementById(errorId).textContent = ""; // clear warn
//         } else {
//             document.getElementById(errorId).textContent = "Is Repeated.";
//         }
//     }
// }
// // add selected author
// document.getElementById("addAuthorButton").addEventListener("click", function () {
//     addToSelectedList("authorInput", "selectedAuthorsList", "duplicateAuthorError", selectedAuthors);
// });
// // add selected Translator
// document.getElementById("addTranslatorButton").addEventListener("click", function () {
//     addToSelectedList("translatorInput", "selectedTranslatorsList", "duplicateTranslatorError", selectedTranslators);
// });

// // add selected Publisher
// document.getElementById("addPublisherButton").addEventListener("click", function () {
//     addToSelectedList("publisherInput", "selectedPublishersList", "duplicatePublisherError", selectedPublishers);
// });
//ـــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ
////////////////////////////////////////////////////////////////////////////////////


// تابع اضافه کردن نویسنده منتخب به لیست
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
                updateHiddenFields(); // اینجا مقادیر فیلدهای مخفی را به‌روز کنید
            };
            listItem.appendChild(removeButton);

            // اضافه کردن مورد به لیست انتخابی
            selectedItemList.push(inputValue);
            selectedList.appendChild(listItem);
            input.value = ""; // پاک کردن مقدار ورودی
            document.getElementById(errorId).textContent = ""; // پاک کردن هشدار
            updateHiddenFields(); // اینجا مقادیر فیلدهای مخفی را به‌روز کنید
        } else {
            document.getElementById(errorId).textContent = "تکراری است.";
        }
    }
}

// تابع برای به‌روز کردن مقادیر فیلدهای مخفی
function updateHiddenFields() {
    var selectedAuthors = selectedAuthors.join(",");
    var selectedPublishers = selectedPublishers.join(",");
    var selectedTranslators = selectedTranslators.join(",");

    console.log("SelectedAuthors: " + selectedAuthors);
    console.log("SelectedPublishers: " + selectedPublishers);
    console.log("SelectedTranslators: " + selectedTranslators);

    document.getElementById("SelectedAuthors").value = selectedAuthors;
    document.getElementById("SelectedPublishers").value = selectedPublishers;
    document.getElementById("SelectedTranslators").value = selectedTranslators;
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