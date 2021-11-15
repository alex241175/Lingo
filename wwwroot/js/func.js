function insertHtmlAtSelectionEnd(html, isBefore) {
    var sel, range, node;
    if (window.getSelection) {
        sel = window.getSelection();
        if (sel.getRangeAt && sel.rangeCount) {
            range = window.getSelection().getRangeAt(0);
            range.collapse(isBefore);

            // Range.createContextualFragment() would be useful here but was
            // until recently non-standard and not supported in all browsers
            // (IE9, for one)
            var el = document.createElement("div");
            el.innerHTML = html;
            var frag = document.createDocumentFragment(), node, lastNode;
            while ( (node = el.firstChild) ) {
                lastNode = frag.appendChild(node);
            }
            range.insertNode(frag);
        }
    } else if (document.selection && document.selection.createRange) {
        range = document.selection.createRange();
        range.collapse(isBefore);
        range.pasteHTML(html);
    }
}

function surroundSelection(textBefore, textAfter) {
    if (window.getSelection) {
        var sel = window.getSelection();
        if (sel.rangeCount > 0) {
            var range = sel.getRangeAt(0);
            var startNode = range.startContainer, startOffset = range.startOffset;

            var startTextNode = document.createTextNode(textBefore);
            var endTextNode = document.createTextNode(textAfter);

            var boundaryRange = range.cloneRange();
            boundaryRange.collapse(false);
            boundaryRange.insertNode(endTextNode);
            boundaryRange.setStart(startNode, startOffset);
            boundaryRange.collapse(true);
            boundaryRange.insertNode(startTextNode);

            // Reselect the original text
            range.setStartAfter(startTextNode);
            range.setEndBefore(endTextNode);
            sel.removeAllRanges();
            sel.addRange(range);
        }
    }
}
  
function insertSurroundingTagAtSelection(){
      var selection = window.getSelection()
      var selection_text = selection.toString()
      var el = document.createElement("span")
      el.className = "vocab";  
      el.innerHTML = `(<span class='index'></span>)<span>${selection_text}</span>`
      // el.textContent = '(' + index + ') ' + selection_text    
      var range = selection.getRangeAt(0)
      range.deleteContents()
      range.insertNode(el)
}