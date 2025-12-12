const ChartManager = {

    chart: null,
    canvasId: null,
    rows: [],
    columns: [],
    columnsDisplay: {},

    //
    // Inicializace správy grafů
    //
    init(canvasId, rows, columns, columnsDisplay) {
        this.canvasId = canvasId;
        this.rows = rows;
        this.columns = columns;
        this.columnsDisplay = columnsDisplay;

        // default fallback – pokud nějaký překlad chybí
        for (const col of columns) {
            if (!this.columnsDisplay[col]) {
                this.columnsDisplay[col] = col;
            }
        }
    },

    //
    // Naplnění dropdownů pro typ grafu a sloupce
    //
    populateSelectors(xSelectId, ySelectId, typeSelectId, defaultX, defaultY, defaultType) {

        const xSel = document.getElementById(xSelectId);
        const ySel = document.getElementById(ySelectId);
        const typeSel = document.getElementById(typeSelectId);

        // dostupné typy grafů
        const types = ["line", "bar", "scatter"];
        for (const t of types) {
            const opt = new Option(t.charAt(0).toUpperCase() + t.slice(1), t);
            typeSel.add(opt);
        }
        typeSel.value = defaultType || "line";

        // dostupné sloupce (přeložené názvy)
        for (const col of this.columns) {
            xSel.add(new Option(this.columnsDisplay[col], col));
            ySel.add(new Option(this.columnsDisplay[col], col));
        }

        xSel.value = defaultX || this.columns[0];
        ySel.value = defaultY || this.columns[0];
    },

    //
    // Vyrobení dat pro graf
    //
    extractData(xCol, yCol) {

        // osa X → text
        const labels = this.rows.map(r => r[xCol]);

        // osa Y → čísla
        const values = this.rows.map(r => {
            const v = r[yCol];
            const num = Number(v);
            return isNaN(num) ? null : num;
        });

        return { labels, values };
    },

    //
    // Hlavní metoda pro vykreslení grafu
    //
    draw(type, xCol, yCol) {

        const ctx = document.getElementById(this.canvasId);

        // zrušíme starý graf
        if (this.chart) {
            this.chart.destroy();
        }

        const { labels, values } = this.extractData(xCol, yCol);
        const xLabel = this.columnsDisplay[xCol] || xCol;
        const yLabel = this.columnsDisplay[yCol] || yCol;

        //
        // Scatter graf má specifická data
        //
        let dataConfig = null;

        if (type === "scatter") {

            const scatterData = this.rows.map(r => ({
                x: Number(r[xCol]),
                y: Number(r[yCol])
            }));

            dataConfig = {
                datasets: [{
                    label: yLabel,
                    data: scatterData,
                    pointRadius: 4
                }]
            };

        } else {

            dataConfig = {
                labels: labels,
                datasets: [{
                    label: yLabel,
                    data: values
                }]
            };
        }

        //
        // vykreslení grafu
        //
        this.chart = new Chart(ctx, {
            type: type,
            data: dataConfig,
            options: {
                responsive: true,

                scales: {
                    x: {
                        title: {
                            display: true,
                            text: xLabel
                        }
                    },
                    y: {
                        title: {
                            display: true,
                            text: yLabel
                        }
                    }
                },

                plugins: {
                    legend: {
                        display: true
                    }
                }
            }
        });
    }
};
