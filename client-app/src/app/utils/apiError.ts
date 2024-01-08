import axios from "axios";
import { toast } from "react-toastify";

export function hanldeApiError(error: unknown) {
    if (axios.isAxiosError(error) && error.response) {
        const message = error.response.data.message;
        toast.error(message);
    } else {
        console.log("An unexpected error occurred");
        toast.error("An unexpected error occurred");
    }
}
