document.addEventListener("DOMContentLoaded", () => {

    const form = document.getElementById("simConfigForm");
    if (!form) return; // bezpečnost pro jiné stránky

    const statusEl = document.getElementById("saveStatus");
    const btnRun = document.getElementById("btnRunSimulation");
    const logEl = document.getElementById("simulationLog");

    const token = form.querySelector('input[name="__RequestVerificationToken"]').value;

    let saveTimer = null;
    let lastSaved = JSON.stringify(getPayload());

    function setStatus(text, canRun) {
        statusEl.textContent = text;
        btnRun.disabled = !canRun;
    }

    function getPayload() {
        function toIntOrNull(v) {
            if (v === "" || v == null) return null;
            const n = parseInt(v, 10);
            return Number.isFinite(n) ? n : null;
        }

        function toDateOrNull(v) {
            return (v === "" || v == null) ? null : v;
        }

        const fd = new FormData(form);
        fd.delete("__RequestVerificationToken");

        const obj = {};
        fd.forEach((v, k) => obj[k] = v);

        obj.StartTime = toDateOrNull(obj.StartTime);
        obj.SimulationTime = toDateOrNull(obj.SimulationTime);
        obj.DeadlineDays = toIntOrNull(obj.DeadlineDays);
        obj.ReplacementCalendarDays = toIntOrNull(obj.ReplacementCalendarDays);
        obj.MaterialLeadTimeHours = toIntOrNull(obj.MaterialLeadTimeHours);

        return obj;
    }

    async function saveConfig() {
        const payload = JSON.stringify(getPayload());
        if (payload === lastSaved) return;

        setStatus("Ukládám…", false);

        try {
            const res = await fetch("/simulation/autosave", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": token
                },
                body: payload
            });

            if (!res.ok) throw new Error();

            lastSaved = payload;
            setStatus("Uloženo ✓", true);
        }
        catch {
            setStatus("Chyba ukládání", false);
        }
    }

    function scheduleSave() {
        clearTimeout(saveTimer);
        saveTimer = setTimeout(saveConfig, 800);
    }

    form.addEventListener("input", scheduleSave);
    form.addEventListener("change", scheduleSave);

    // --- spuštění simulace ---
    btnRun.addEventListener("click", async () => {
        btnRun.disabled = true;
        logEl.textContent = "Spouštím simulaci...\n";

        try {
            const response = await fetch("/simulation/run", { method: "POST" });
            const lines = await response.json();
            logEl.textContent = lines.join("\n");
        }
        catch (err) {
            logEl.textContent += "Výjimka: " + err;
        }
        finally {
            btnRun.disabled = false;
        }
    });

});
