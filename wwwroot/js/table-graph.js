// GLOBAL SCRIPT FOR ALL TABLES
// Requires: Bootstrap Table + Chart.js

$(function () {
    if (!window.currentTableModel) {
        console.warn("No table model found.");
        return;
    }

    const rows = window.currentTableModel.rows;
    const columns = window.currentTableModel.columns;
    const tableName = window.currentTableModel.tableName;

    // ============================================================
    // 1) INITIALIZACE BOOTSTRAP TABLE
    // ============================================================

    $("#dynTable").bootstrapTable();

    // ============================================================
    // 2) INLINE EDITACE
    // ============================================================

    let originalValue = "";

    $("#dynTable tbody").on("dblclick", "td", function () {
        const cell = $(this);
        const col = cell.data("col");
        const id = cell.data("id");

        if (col === "Id") return;

        // === ZÍSKÁNÍ PŮVODNÍ HODNOTY (KRITICKÉ) ===
        let originalValue = cell.text().trim();

        // Pokud je hodnota prázdná, zkusíme ji získat z interní BT datové struktury
        if (!originalValue) {
            const rowIndex = cell.closest("tr").data("index");
            if (rowIndex !== undefined) {
                const rowData = $("#dynTable").bootstrapTable("getData")[rowIndex];
                if (rowData && rowData[col] !== undefined) {
                    originalValue = rowData[col];
                }
            }
        }

        // === VLOŽENÍ INPUTU SE SPRÁVNOU HODNOTOU ===
        const input = $("<input type='text' class='form-control form-control-sm'>")
            .val(originalValue);

        cell.empty().append(input);
        input.focus();

        // ENTER → blur
        input.on("keydown", function (e) {
            if (e.key === "Enter") input.blur();
            if (e.key === "Escape") cell.text(originalValue);
        });

        // BLUR → uložíme změnu
        input.on("blur", function () {
            const newValue = input.val().trim();

            if (newValue === originalValue) {
                cell.text(originalValue);
                return;
            }

            const updateData = {
                tableName: window.currentTableModel.tableName,
                row: {
                    Id: id
                }
            };
            updateData.row[col] = newValue;

            fetch("/Database/UpdateCell", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(updateData)
            })
                .then(r => {
                    if (!r.ok) {
                        alert("Update failed");
                        cell.text(originalValue);
                        return;
                    }

                    // Aktualizujeme UI i interní data BT
                    cell.text(newValue);

                    const rowIndex = cell.closest("tr").data("index");
                    if (rowIndex !== undefined) {
                        $("#dynTable").bootstrapTable("updateCell", {
                            index: rowIndex,
                            field: col,
                            value: newValue
                        });
                    }
                });
        });
    });

    // ============================================================
    // 3) GRAF – AUTOMATICKÁ DETEKCE NUM SLUPCŮ
    // ============================================================

    function isNumeric(colName) {
        for (let r of rows) {
            const val = r[colName];
            if (val !== null && val !== "" && !isNaN(Number(val))) return true;
        }
        return false;
    }

    const numericCols = columns.filter(c => isNumeric(c));

    if (numericCols.length >= 2) {

        const xCol = numericCols[0];
        const yCol = numericCols[1];

        const labels = rows.map(r => r[xCol]);
        const values = rows.map(r => Number(r[yCol]));

        const ctx = document.getElementById("chartCanvas").getContext("2d");

        new Chart(ctx, {
            type: "line",
            data: {
                labels: labels,
                datasets: [{
                    label: `${yCol} podle ${xCol}`,
                    data: values,
                    borderWidth: 2,
                    borderColor: "#007bff",
                    backgroundColor: "rgba(0, 123, 255, 0.25)",
                    tension: 0.2
                }]
            },
            options: {
                responsive: true,
                scales: {
                    x: { title: { display: true, text: xCol } },
                    y: { title: { display: true, text: yCol } }
                }
            }
        });
    } else {
        console.warn("Pro graf nejsou dostupné alespoň 2 numerické sloupce.");
    }
});
