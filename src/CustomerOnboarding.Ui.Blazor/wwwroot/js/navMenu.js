window.setupClickOutside = (element, dotnetHelper) => {
    function handleClickOutside(event) {
        if (element && !element.contains(event.target)) {
            dotnetHelper.invokeMethodAsync('HandleClickOutside');
        }
    }

    document.addEventListener('mousedown', handleClickOutside);

    // Clean up the event listener when component is disposed
    return {
        dispose: () => {
            document.removeEventListener('mousedown', handleClickOutside);
        }
    };
};