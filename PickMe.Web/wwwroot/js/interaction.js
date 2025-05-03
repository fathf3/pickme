async function sendPostRequest(url, data = {}, requireAuth = true) {
    var isAuthenticated = window.isAuthenticated; // Razor'dan gelecek
    if (requireAuth && !isAuthenticated) {
        Swal.fire("Etkileşime girmek veya anket oluşturmak için lütfen giriş yapınız.");
        return null;
    }

    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    const headers = {
        'Content-Type': 'application/json'
    };

    if (token) {
        headers['RequestVerificationToken'] = token.value;
    }

    const response = await fetch(url, {
        method: 'POST',
        headers,
        body: JSON.stringify(data)
    });

    if (response.ok) {
        try {
            return await response.json();
        } catch {
            return true;
        }
    } else {
        console.error("Hata:", response.status);
        return null;
    }
}

async function toggleLike(event, surveyId) {
    event.preventDefault();

    const result = await sendPostRequest(`/Survey/ToggleLike?surveyId=${surveyId}`, {}, true);
    if (result !== null) {
        const form = document.getElementById(`likeForm-${surveyId}`);
        const button = form.querySelector('button');
        const icon = button.querySelector('i');
        const countSpan = button.querySelector('span');

        if (button.classList.contains('btn-outline-danger')) {
            button.classList.remove('btn-outline-danger');
            button.classList.add('btn-danger');
        } else {
            button.classList.remove('btn-danger');
            button.classList.add('btn-outline-danger');
        }

        if (countSpan) {
            let count = parseInt(countSpan.innerText);
            count += button.classList.contains('btn-danger') ? 1 : -1;
            countSpan.innerText = count;
        }
    }
}

async function voteImage(surveyId, isFirstImage) {
    const result = await sendPostRequest('/Survey/Vote', { surveyId, isFirstImage }, true);
    if (result && result.success) {
        Swal.fire("Oyunuz kaydedildi.");
        // İstersen başka bir şey de yapabilirsin
    } else if (result && result.message) {
        Swal.fire(result.message);
    }
}

async function addComment(event, surveyId) {
    event.preventDefault();
    const content = document.getElementById(`commentContent-${surveyId}`).value;

    const result = await sendPostRequest('/Survey/AddComment', { surveyId, content }, true);
    if (result !== null) {
        location.reload();
    }
}
function setReportSurveyId(surveyId) {
    document.getElementById('reportSurveyId').value = surveyId;
}

async function submitReport(event) {
    event.preventDefault();

    const surveyId = document.getElementById('reportSurveyId').value;
    const reason = document.getElementById('reportReason').value;

    const result = await sendPostRequest('/Survey/Report', { surveyId, reason }, true);
    if (result !== null) {
        const modal = bootstrap.Modal.getInstance(document.getElementById('reportModal'));
        modal.hide();
        Swal.fire("Şikayetiniz başarıyla gönderildi.");
        document.getElementById('reportReason').value = '';
    }
}