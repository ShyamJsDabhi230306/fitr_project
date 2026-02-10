/* ==========================================
   FITR CLIENT-SIDE VALIDATION
   ========================================== */

document.addEventListener("DOMContentLoaded", function () {
    const forms = document.querySelectorAll(".needs-validation");

    forms.forEach(form => {
        // 1. Initial State: Disable Button
        toggleSubmitButton(form);

        // 2. Add Listeners to all inputs
        form.addEventListener("input", () => toggleSubmitButton(form));
        form.addEventListener("change", () => toggleSubmitButton(form));

        // 3. Prevent Default Submit if Invalid (Backup)
        form.addEventListener("submit", function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add("was-validated");
        });
    });

    // 4. Special Handling: Duplicate DC Check (Basic Tab)
    setupDuplicateCheck();

    // 5. Special Handling: Dynamic Rows (Material, SrData, Visual)
    setupDynamicObservers();
});

/* ----------------------------------------------------------------
   TOGGLE SUBMIT BUTTON
   ---------------------------------------------------------------- */
function toggleSubmitButton(form) {
    const btn = form.querySelector("button[type='submit']");
    if (!btn) return;

    if (form.checkValidity()) {
        btn.removeAttribute("disabled");
        btn.classList.remove("btn-secondary"); // Optional style
        // btn.classList.add("btn-primary");
    } else {
        btn.setAttribute("disabled", "disabled");
        // btn.classList.remove("btn-primary");
        btn.classList.add("btn-secondary"); // Optional visual cue
    }
}

/* ----------------------------------------------------------------
   DUPLICATE DC CHECK (AJAX)
   ---------------------------------------------------------------- */
function setupDuplicateCheck() {
    const dcNoInput = document.querySelector("#Master_DCNo");
    const fitrIdInput = document.querySelector("#Master_FitrId");

    if (!dcNoInput) return;

    const checkDuplicate = async () => {
        const dcNo = dcNoInput.value.trim();
        const fitrId = fitrIdInput ? fitrIdInput.value : 0;

        // Reset validity
        dcNoInput.setCustomValidity("");
        dcNoInput.classList.remove("is-invalid");

        if (!dcNo) return true; // Let 'required' handle empty

        try {
            const response = await fetch(`/Fitr/CheckDuplicateDc?dcNo=${encodeURIComponent(dcNo)}&fitrId=${fitrId}`);
            const result = await response.json();

            if (result !== true) {
                // Duplicate found
                dcNoInput.setCustomValidity(result);
                dcNoInput.classList.add("is-invalid");

                // Show Error UI
                const feedback = dcNoInput.nextElementSibling;
                if (feedback && feedback.classList.contains("invalid-feedback")) {
                    feedback.textContent = result;
                }
                return false;
            } else {
                // Valid
                dcNoInput.setCustomValidity("");
                dcNoInput.classList.remove("is-invalid");
                const feedback = dcNoInput.nextElementSibling;
                if (feedback && feedback.classList.contains("invalid-feedback")) {
                    feedback.textContent = "Please enter DC number"; // Restore default
                }
                return true;
            }
        } catch (err) {
            console.error("Duplicate check failed", err);
            return true; // Fail open on error?
        } finally {
            toggleSubmitButton(dcNoInput.closest("form"));
        }
    };

    dcNoInput.addEventListener("blur", checkDuplicate);
    dcNoInput.addEventListener("input", () => {
        // Clear error as user types to feel responsive
        dcNoInput.setCustomValidity("");
        dcNoInput.classList.remove("is-invalid");
    });

    // 🔥 Intercept Submit to Focus on DC Error
    const form = dcNoInput.closest("form");
    if (form) {
        form.addEventListener("submit", async function (e) {
            // Re-run check just in case it's a fast click
            const isValid = await checkDuplicate();

            if (!isValid) {
                e.preventDefault();
                e.stopPropagation();
                dcNoInput.focus();

                // Professional UI: Scroll to error
                dcNoInput.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
        });
    }
}

/* ----------------------------------------------------------------
   DYNAMIC ROW OBSERVER
   ---------------------------------------------------------------- */
function setupDynamicObservers() {
    // We observe the TABLES where rows are added
    const tableIds = ["#materialTable", "#srTable", "#visualTable"];

    tableIds.forEach(id => {
        const table = document.querySelector(id);
        if (table) {
            const observer = new MutationObserver((mutations) => {
                const form = table.closest("form");
                if (form) {
                    // Re-attach listeners to NEW inputs
                    form.querySelectorAll("input, select").forEach(input => {
                        // Avoid adding duplicate listeners (simplified approach: remove/add or just add, browser handles dupes usually fine for named functions, but anon ones stack. 
                        // Safer: just re-trigger validation check)
                        input.removeEventListener("input", () => toggleSubmitButton(form));
                        input.addEventListener("input", () => toggleSubmitButton(form));
                    });

                    // Force Check
                    toggleSubmitButton(form);

                    // Ensure 'required' is set on new rows if they are missing it (fallback)
                    // But we will try to add 'required' in the View HTML template logic.
                }
            });

            observer.observe(table, { childList: true, subtree: true });
        }
    });

    // Also observe global button clicks that might add rows outside table (helper triggers)
    // Actually MutationObserver covering the table body is sufficient.
}
