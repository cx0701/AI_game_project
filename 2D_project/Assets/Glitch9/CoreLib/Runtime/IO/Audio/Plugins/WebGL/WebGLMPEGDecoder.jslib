mergeInto(LibraryManager.library, {
    DecodeMPEGFromBase64: function (base64Ptr, targetPtr, methodPtr) {
        const base64 = UTF8ToString(base64Ptr);
        const target = UTF8ToString(targetPtr);
        const method = UTF8ToString(methodPtr);

        // window에 정의된 async 함수 호출
        if (typeof window.decodeMp3Async === 'function') {
            window.decodeMpegAsync(base64, target, method);
        } else {
            console.error("window.decodeMpegAsync is not defined");
        }
    }
});
