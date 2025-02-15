document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".vote-button").forEach(function (element) {
        element.addEventListener("click", function () {
            const surveyId = this.getAttribute("data-survey-id");
            const isFirstImage = this.getAttribute("data-image") === "1";

            fetch("/Survey/Vote", {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                    "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: `surveyId=${surveyId}&isFirstImage=${isFirstImage}`
            })
                .then(response => response.json())
                .then(data => {
                    const messageContainer = document.getElementById("message-container");

                    if (data.success) {
                        // Başarı mesajı
                        messageContainer.textContent = "Oyunuz başarıyla kaydedildi!";
                        messageContainer.className = "alert-message success-message"; // Başarı stilini uygula
                    } else {
                        // Hata mesajı
                        messageContainer.textContent = "Hata: " + data.message;
                        messageContainer.className = "alert-message error-message"; // Hata stilini uygula
                    }

                    // Mesajı göster
                    messageContainer.style.display = "block";

                    // 5 saniye sonra mesajı gizle
                    setTimeout(() => {
                        messageContainer.style.display = "none";
                    }, 5000);
                })
                .catch(error => console.error("Hata:", error));
        });
    });
});
