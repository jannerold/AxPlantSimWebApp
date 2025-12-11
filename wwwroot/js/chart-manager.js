// ---------------------------------------------------------------
//  CHART MANAGER – GLOBAL GRAPH ENGINE FOR THE WHOLE APPLICATION
// ---------------------------------------------------------------

window.ChartManager = {
    chart: null,
    rows: [],
    columns: [],
    ctx: null,

    // Initialize chart engine
    init(canvasId, rows, columns) {
        this.rows = rows;
        this.columns = columns;
        this.ctx = document.getElementById(canvasId).getContext("2d");
    },

    // Populate dropdowns globally
    populateSelectors(xSelectId, ySelectId, typeSelectId, selectedX, selectedY, selectedType) {

        const xSel = document.getElementById(xSelectId);
        const ySel = document.getElementById(ySelectId);
        const tSel = document.getElementById(typeSelectId);

        // graph types
        tSel.innerHTML = `
      <option value="line">Line</option>
      <option value="bar">Bar</option>
      <option value="scatter">Scatter</option>
    `;

        // columns for x + y
        xSel.innerHTML = "";
        ySel.innerHTML = "";

        this.columns.forEach(c => {
            xSel.innerHTML += `<option value="${c}">${c}</option>`;
            ySel.innerHTML += `<option value="${c}">${c}</option>`;
        });

        // restore incoming values
        if (selectedX) xSel.value = selectedX;
        if (selectedY) ySel.value = selectedY;
        if (selectedType) tSel.value = selectedType;
    },

    // Draw or redraw graph
    draw(chartType, xCol, yCol) {

        // Data
        const labels = this.rows.map(r => r[xCol]);
        const values = this.rows.map(r => Number(r[yCol]));

        // Destroy previous chart
        if (this.chart) this.chart.destroy();

        // Create new one
        this.chart = new Chart(this.ctx, {
            type: chartType,
            data: {
                labels: labels,
                datasets: [{
                    label: `${yCol} podle ${xCol}`,
                    data: values,
                    borderColor: "#007bff",
                    backgroundColor: "rgba(0, 123, 255, 0.25)",
                    borderWidth: 2,
                    pointRadius: chartType === "scatter" ? 4 : 2,
                    showLine: chartType !== "scatter"
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
    }
};
