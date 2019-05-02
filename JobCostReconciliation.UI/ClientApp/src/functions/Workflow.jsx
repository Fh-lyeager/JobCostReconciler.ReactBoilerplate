
export default function WorkflowData() {
    fetch('api/Workflow')
        .then(response => response.json())
        .then(data => {
             return data;
        });
}