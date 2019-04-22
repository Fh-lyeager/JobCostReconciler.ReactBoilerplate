export default function renderPurchaseOrderLastRun()
{
    fetch('api/PurchaseOrderQueue/LastRun')
        .then(response => response.json())
        .then(data => {
            this.setState({ lastRun: Array.from(data) });
        });

    let lastRun = this.state.lastRun;

    return (
        {lastRun}
    );
}