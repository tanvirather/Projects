import { apiClient, toasterStore, storage } from "../lib/index.js";
import { AccountStore } from "./accountStore.js";
import { NumericTypeStore } from "./numericTypeStore.js";

export const accountStore = new AccountStore(apiClient, storage);
export const numericTypeStore = new NumericTypeStore(apiClient);
export { toasterStore, storage };
