
document.addEventListener("DOMContentLoaded", function () {
    const forms = document.querySelectorAll(".needs-validation");

    forms.forEach(form => {
        const submitBtn = form.querySelector('button[type="submit"]');

        // Initial Layout
        toggleButtonState(form, submitBtn);

        // Input listener
        form.addEventListener("input", () => {
            toggleButtonState(form, submitBtn);
        });

        // Change listener (for selects)
        form.addEventListener("change", () => {
            toggleButtonState(form, submitBtn);
        });

        // Submit listener to show validation styles
        form.addEventListener("submit", event => {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add("was-validated");
        });
    });

    // Observer for dynamic content (adding rows in SrData)
    const observer = new MutationObserver((mutations) => {
        let shouldCheck = false;
        let targetForm = null;

        mutations.forEach((mutation) => {
            if (mutation.type === 'childList') {
                const form = mutation.target.closest("form");
                if (form && form.classList.contains("needs-validation")) {
                    shouldCheck = true;
                    targetForm = form;
                }
            }
        });

        if (shouldCheck && targetForm) {
            const submitBtn = targetForm.querySelector('button[type="submit"]');
            toggleButtonState(targetForm, submitBtn);
        }
    });

    // Observe specific tables where rows are added
    const srTable = document.getElementById("srTable");
    if (srTable) {
        const tbody = srTable.querySelector("tbody");
        if (tbody) {
            observer.observe(tbody, { childList: true, subtree: true });
        }
    }
});

function toggleButtonState(form, btn) {
    if (!btn) return;

    if (form.checkValidity()) {
        btn.removeAttribute("disabled");
    } else {
        btn.setAttribute("disabled", "disabled");
    }
}
