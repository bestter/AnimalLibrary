import React, { Component } from 'react';

export class CreateTaxonomicRank extends Component {
    render() {
        return (
            <form>
                <div class="form-group">
                    <label for="taxonomicRankName">Email address</label>
                    <input type="text" class="form-control" id="taxonomicRankName" aria-describedby="taxonomicRankNameHelp" placeholder="Enter email" />
                    <small id="taxonomicRankNameHelp" class="form-text text-muted">Nom du rang taxonomique.</small>
                </div>
                <div class="form-group">
                    <label for="taxonomicRankTypeID">Example select</label>
                    <select class="form-control" id="taxonomicRankTypeID">
                        <option value="1">Pamplemousse</option>
                        <option value="2">Citron vert</option>
                        <option value="3">Noix de coco</option>
                        <option selected value="4">Mangue</option>
                    </select>
                </div>
                <div class="form-check">
                    <input type="checkbox" class="form-check-input" id="exampleCheck1" />
                    <label class="form-check-label" for="exampleCheck1">Check me out</label>
                </div>
                <button type="submit" class="btn btn-primary">Submit</button>
            </form>
        );
    }
}