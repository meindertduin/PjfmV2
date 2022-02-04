import { Validators } from '@angular/forms';

/**
 * The same as Validators.required, but this prevents the unbound-method eslint error from popping up everywhere.
 * Read https://github.com/typescript-eslint/typescript-eslint/issues/1929 for more info
 * **/
// eslint-disable-next-line @typescript-eslint/unbound-method
export const requiredValidator = Validators.required;
