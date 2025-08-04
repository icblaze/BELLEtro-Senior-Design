mergeInto(LibraryManager.library, {
    setUserIsPlayingGame: function (state) {
        try {
            window.dispatchReactUnityEvent("setUserIsPlayingGame", state);
        } catch (e) {
            console.warn("Failed to dispatch React event 'setUserIsPlayingGame' from Unity.");
        }
    },
    setSessionID: function (sessionID) {
        try {
            window.dispatchReactUnityEvent("setSessionID", UTF8ToString(sessionID));
        } catch (e) {
            console.warn("Failed to dispatch React event 'setSessionID' from Unity.");
        }
    },
    setCurrentAnte: function (score) {
        try {
            window.dispatchReactUnityEvent("setCurrentAnte", score);
        } catch (e) {
            console.warn("Failed to dispatch React event 'setCurrentAnte' from Unity.");
        }
    },
    setMostFrequentHand: function (mostFrequentHand) {
        try {
            window.dispatchReactUnityEvent("setMostFrequentHand", mostFrequentHand);
        } catch (e) {
            console.warn("Failed to dispatch React event 'setMostFrequentHand' from Unity.");
        }
    }
});