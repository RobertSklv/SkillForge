import { createURLSearchParams } from "./api/client";
import DefaultGridState from "./types/DefaultGridState";
import GridState from "./types/GridState";

function getUpdatedState(param: string, value: any, currentState: GridState, defaultState: DefaultGridState): GridState {
    let stateCopy = JSON.parse(JSON.stringify(currentState));
    stateCopy[param] = value;

    if (stateCopy.p === defaultState.p) {
        delete stateCopy.p;
    }
    if (stateCopy.limit === defaultState.limit) {
        delete stateCopy.limit;
    }
    if (stateCopy.q === defaultState.q) {
        delete stateCopy.q;
    }
    if (stateCopy.sortBy === defaultState.sortBy) {
        delete stateCopy.sortBy;
    }
    if (stateCopy.sortOrder === defaultState.sortOrder) {
        delete stateCopy.sortOrder;
    }

    return stateCopy;
}

export function generateSearchQuery(param: string, value: any, currentState: GridState, defaultState: DefaultGridState): string {

    let updatedState = getUpdatedState(param, value, currentState, defaultState);
    let query = createURLSearchParams(updatedState).toString();

    return '?' + query;
}