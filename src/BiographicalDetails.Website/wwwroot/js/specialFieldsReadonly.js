function loadReadOnlyFields() {
    makeReadOnlyIfHasValueAndNoError('SocialInsuranceNumber');
    makeReadOnlyIfHasValueAndNoError('UniqueClientIdentifier');
}

function makeReadOnlyIfHasValueAndNoError(fieldName) {
    let inputElement = $(`input#${fieldName}`)[0];
    if (inputElement != null) {
        if (!isStringNullOrEmpty(inputElement.value) && !fieldHasError(fieldName)) {
            inputElement.readOnly = true;
            inputElement.style.backgroundColor = 'lightgrey';
        }
    }
}

loadReadOnlyFields();