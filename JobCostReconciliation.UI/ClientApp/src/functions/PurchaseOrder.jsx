

export default function PurchaseOrder() {

    //getPurchaseOrderLastRun() {
        fetch('api/PurchaseOrderQueue/LastRun')
            .then(response => response.json())
            .then(data => {
                this.setState({ lastRun: Array.from(data), loading: false });
            });
    //}

}
