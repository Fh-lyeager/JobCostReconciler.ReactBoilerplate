

class PurchaseOrder {

    getPurchaseOrderLastRun() {
        fetch('api/PurchaseOrderQueue/LastRun')
            .then(response => response.json())
            .then(data => {
                this.setState({ lastRun: Array.from(data), loading: false });
            });
    }
}



export default (JobCostSummaryPage);