

const templateTest = `
    <div id="templateTest" class="card" style="width: 18rem;">
        <div class="card-body">
            <h5 class="card-title">卡片資料：{{ data }}</h5>
        </div>
        <div>
            <button v-on:click="AddCount()">+1</button>
        </div>
    </div>
`;