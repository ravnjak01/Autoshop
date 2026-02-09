import { FormBuilder, FormGroup } from '@angular/forms';
import { priceRangeValidator } from './price-range.validator';

describe('priceRangeValidator', () => {
  let form: FormGroup;

  beforeEach(() => {
    const fb = new FormBuilder();

    form = fb.group(
      {
        minPrice: [0],
        maxPrice: [0]
      },
      { validators: priceRangeValidator }
    );
  });

  it('should be valid when maxPrice is greater than or equal to minPrice', () => {
    form.setValue({ minPrice: 100, maxPrice: 200 });

    expect(form.valid).toBeTrue();
    expect(form.errors).toBeNull();
  });

  it('should be invalid when maxPrice is less than minPrice', () => {
    form.setValue({ minPrice: 200, maxPrice: 100 });

    expect(form.valid).toBeFalse();
    expect(form.errors).toEqual({ invalidPriceRange: true });
  });
});
