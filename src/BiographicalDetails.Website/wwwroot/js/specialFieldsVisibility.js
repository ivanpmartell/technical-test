const immigrationStatusDict = Object.freeze({
  CanadianCitizen: '0',
  PermanentResident: '1',
  TemporaryForeignWorker: '2',
  InternationalStudent: '3',
  ProtectedPerson: '4',
  Indigenous: '5',
  Visitor: '6'
});

function toggleSpecialFields(immigrationStatus) {
  switch (immigrationStatus) {
    case immigrationStatusDict.CanadianCitizen:
      changeVisibility(true, false);
      break;
    case immigrationStatusDict.PermanentResident:
      changeVisibility(true, true);
      break;
    case immigrationStatusDict.TemporaryForeignWorker:
      changeVisibility(true, true);
      break;
    case immigrationStatusDict.InternationalStudent:
      changeVisibility(true, true);
      break;
    case immigrationStatusDict.ProtectedPerson:
      changeVisibility(true, true);
      break;
    case immigrationStatusDict.Indigenous:
      changeVisibility(true, false);
      break;
    case immigrationStatusDict.Visitor:
      changeVisibility(false, true);
      break;
    default:
      changeVisibility(true, true);
      break;
  }
}

function changeVisibility(sinVisibility, uciVisibility) {
  setElementVisibility($('#socialInsuranceNumberGroup'), sinVisibility);
  setElementVisibility($('#uniqueClientIdentifierGroup'), uciVisibility);
}

function setElementVisibility(element, visibility) {
  return visibility ? element.show() : element.hide();
}



function loadVisibility(){
  toggleSpecialFields($('select#ImmigrationStatus')[0].value)
    
  let sinElement = $('#socialInsuranceNumberGroup');
  let uciElement = $('#uniqueClientIdentifierGroup');
  if (shouldElementDisplay('SocialInsuranceNumber')) {
    setElementVisibility(sinElement, true);
  }
  if (shouldElementDisplay('UniqueClientIdentifier')) {
    setElementVisibility(uciElement, true);
  }
}

function shouldElementDisplay(fieldName) {
  let inputElement = $(`input#${fieldName}`)[0];
  if (inputElement != null) {
      if (!isStringNullOrEmpty(inputElement.value)) {
          return true;
      }
  }

  return $(`span[data-valmsg-for=${fieldName}].field-validation-error:visible`).length > 0;
}

function isStringNullOrEmpty(str) {
    if (str == null)
        return true;
    return str === ''
}

//Run on page load
loadVisibility();