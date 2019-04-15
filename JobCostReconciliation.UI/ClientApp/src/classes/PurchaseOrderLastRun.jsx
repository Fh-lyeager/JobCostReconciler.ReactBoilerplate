import React from "react";

export default function renderPurchaseOrderLastRun() {

    //render() {
        fetch('api/PurchaseOrderQueue/LastRun')
            .then(response => response.json())
            .then(data => {
                this.setState({ lastRun: Array.from(data), loading: false });
            });

        let lastRun = this.state.lastRun;

        return (
            {lastRun}
        );
    //}
}

//export default (renderPurchaseOrderLastRun);