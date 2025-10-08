let products = [];
let cart = { items: [] };
let orders = [];

let BASE_URL = '';

async function initializeApp() {
    try {
        const response = await fetch('/config.json');
        const config = await response.json();
        BASE_URL = config.API_BASE_URL;

        // Now BASE_URL is ready — fetch initial data:
        await fetchProducts();
        await fetchCart();

        // Set up event listeners that may use BASE_URL or fetched data
        document.getElementById('toggle-past-orders').addEventListener('click', async () => {
            const list = document.getElementById('past-orders-list');
            const btn = document.getElementById('toggle-past-orders');

            const isHidden = list.classList.toggle('hidden');
            btn.setAttribute('aria-expanded', !isHidden);
            btn.textContent = isHidden ? 'Show Past Orders ▼' : 'Hide Past Orders ▲';

            if (!isHidden) {
                list.innerHTML = '<p>Loading past orders...</p>';
                await fetchOrders();
                renderOrders(orders);
            }
        });

    } catch (error) {
        console.error('Failed to load config:', error);
        alert('Failed to load config, please try again later.');
    }
}

const fetchProducts = async () => {
    try {
        const response = await fetch(`${BASE_URL}/api/products`);
        if (!response.ok) throw new Error('Failed to fetch products');
        products = await response.json();
        renderProducts();
    } catch (error) {
        console.error('Error fetching products:', error);
        alert('Error fetching product data.');
    }
};

const fetchCart = async () => {
    try {
        const response = await fetch(`${BASE_URL}/api/basket`);
        if (!response.ok) throw new Error('Failed to fetch basket');
        cart = await response.json();
        updateCart();
    } catch (error) {
        console.error('Error fetching basket:', error);
        alert('Error fetching basket data.');
    }
};

const fetchOrders = async () => {
    try {
        const response = await fetch(`${BASE_URL}/api/order`);
        if (!response.ok) throw new Error('Failed to fetch order');
        orders = await response.json();
        renderOrders();
    } catch (error) {
        console.error('Error fetching orders:', error);
        alert('Error fetching orders data.');
    }
};

const renderOrders = (orders) => {
    const list = document.getElementById('past-orders-list');

    if (!orders || orders.length === 0) {
        list.innerHTML = '<p>No past orders found.</p>';
        return;
    }

    list.innerHTML = ''; // Clear previous content

    orders.forEach(order => {
        const orderEl = document.createElement('div');
        orderEl.className = 'past-order-item';

        const orderDate = new Date(order.orderDate).toLocaleDateString();
        const itemsSummary = order.items
            .map(item =>
                `<strong>${item.productName}</strong>: ` +
                `Unit ${item.unitFinalPrice}€ (Base ${item.unitBasePrice}€), ` +
                `Qty ${item.quantity}, ` +
                `Total ${item.totalFinalPrice}€ (Base ${item.totalBasePrice}€)`
            )
            .join('<br>');

        orderEl.innerHTML = `
            <strong>Order #${order.orderId}</strong> - ${orderDate}<br>
            Items:<br> ${itemsSummary}<br>
            Total: ${order.finalPrice.toFixed(2)}€ out of ${order.basePrice.toFixed(2)}€
        `;

        list.appendChild(orderEl);
    });
};

const renderProducts = () => {
    const productList = document.getElementById('product-list');
    productList.innerHTML = '';

    products.forEach(product => {
        const productElement = document.createElement('div');
        productElement.className = 'product';

        const price = Number(product.price);
        const baseprice = Number(product.basePrice);

        productElement.innerHTML = `
			<div class="product-image">
				<img src="${product.imageUrl ? BASE_URL + '/' + product.imageUrl.replace(/^~\//, '') : ''}" alt="${product.name}">
			</div>
			<h3>${product.name}</h3>
			${!isNaN(baseprice) && !isNaN(price) && baseprice !== price
                ? `<p><s>${baseprice.toFixed(2)} €</s> <strong>${price.toFixed(2)} €</strong></p>`
                : !isNaN(price)
                    ? `<p>${price.toFixed(2)} €</p>`
                    : `<p>Price not available</p>`
            }
			<button onclick="addToCart(${product.productId})">Add to Cart</button>
		`;

        // Render discounts
        if (product.discounts?.length > 0) {
            const discountsElement = document.createElement('div');
            discountsElement.className = 'discount';
            discountsElement.innerHTML = `
                <strong>Discounts:</strong>
                <ul>
                    ${product.discounts.map(discount => `
                        <li>
                            <div><strong>${discount.description || discount.event?.description || 'No description'}</strong></div>
                            <div>Valid: ${new Date(discount.event.startDate).toLocaleDateString()} - ${new Date(discount.event.endDate).toLocaleDateString()}</div>
                        </li>
                    `).join('')}
                </ul>
            `;
            productElement.appendChild(discountsElement);
        }

        // Render promotions
        if (product.promotions?.length > 0) {
            const promotionsElement = document.createElement('div');
            promotionsElement.className = 'discount';
            promotionsElement.innerHTML = `
                <strong>Promotions:</strong>
                <ul>
                    ${product.promotions.map(promo => `
                        <li>
                            <div><strong>${promo.discount.description || promo.event?.description || 'No description'}</strong></div>
                            <div>Valid: ${new Date(promo.event.startDate).toLocaleDateString()} - ${new Date(promo.event.endDate).toLocaleDateString()}</div>
                        </li>
                    `).join('')}
                </ul>
            `;
            productElement.appendChild(promotionsElement);
        }

        productList.appendChild(productElement);
    });
};

const addToCart = async (id) => {
    const product = products.find(p => p.productId === id);
    if (!product) return;

    const existing = cart.items.find(item => item.productId === id);

    try {
        if (existing) {
            const updatedItem = { productId: id, quantity: existing.quantity + 1 };
            const success = await updateCartBackend(updatedItem);
            if (!success) throw new Error('Add failed');
        } else {
            const newItem = { productId: id, quantity: 1 };
            const success = await addToCartBackend(newItem);
            if (!success) throw new Error('Add failed');
        }
        await fetchCart();
    } catch (error) {
        console.error('Add to cart error:', error);
        alert('Failed to add item to cart.');
    }
};

const removeFromCart = async (id) => {
    const existing = cart.items.find(i => i.productId === id);
    if (!existing) return;

    try {
        const success = await removeFromCartBackend(existing);
        if (!success) throw new Error('Remove failed');
        await fetchCart();
    } catch (error) {
        console.error('Remove from cart error:', error);
        alert('Failed to remove item from cart.');
    }
};

const updateQuantity = async (id, delta) => {
    const item = cart.items.find(i => i.productId === id);
    if (!item) return;

    const newQuantity = item.quantity + delta;

    try {
        if (newQuantity <= 0) {
            await removeFromCart(id);
        } else {
            const success = await updateCartBackend({ productId: id, quantity: newQuantity });
            if (!success) throw new Error('Update failed');
            await fetchCart();
        }
    } catch (error) {
        console.error('Update quantity error:', error);
        alert('Failed to update item quantity.');
    }
};

const setQuantity = async (id, newQuantity) => {
    newQuantity = parseInt(newQuantity);
    if (isNaN(newQuantity) || newQuantity < 0) {
        alert('Please enter a valid quantity (1 or more).');
        // Optionally reset input to previous valid quantity here
        await fetchCart(); // reload cart to reset UI
        return;
    }

    try {
        const item = cart.items.find(i => i.productId === id);
        if (!item) return;

        const delta = newQuantity - item.quantity;
        if (delta !== 0) {
            await updateQuantity(id, delta);
        }
    } catch (error) {
        console.error('Set quantity error:', error);
        alert('Failed to update item quantity.');
    }
};

const updateCart = () => {
    const cartElement = document.getElementById('cart');
    cartElement.innerHTML = '';
    let totalPrice = 0;

    cart.items.forEach(item => {
        const product = products.find(p => p.productId === item.productId);
        if (!product) return;

        const price = Number(item.finalPrice);
        const basePrice = Number(item.basePrice);

        const cartItemElement = document.createElement('div');
        cartItemElement.className = 'cart-item';

        let priceHtml = '';
        if (!isNaN(basePrice) && !isNaN(price)) {
            if (basePrice !== price) {
                priceHtml = `<p>Price: <s>${basePrice.toFixed(2)} €</s> <strong>${price.toFixed(2)} €</strong></p>`;
            } else {
                priceHtml = `<p>Price: ${price.toFixed(2)} €</p>`;
            }
        } else {
            priceHtml = `<p>Price: Not available</p>`;
        }

        cartItemElement.innerHTML = `
            <h3>${product.name}</h3>
            ${priceHtml}
            <div class="cart-item-actions">
                <div class="cart-item-quantity-controls">
                    <button onclick="updateQuantity(${item.productId}, -1)">-</button>
                    <input type="number" min="0" value="${item.quantity}" onchange="setQuantity(${item.productId}, this.value)" />
                    <button onclick="updateQuantity(${item.productId}, 1)">+</button>
                </div>
                <div class="cart-item-remove">
                    <button onclick="removeFromCart(${item.productId})">Remove</button>
                </div>
            </div>
        `;

        cartElement.appendChild(cartItemElement);
        totalPrice += price;
    });

    document.getElementById('total-price').textContent = totalPrice.toFixed(2);
};

const addToCartBackend = async (cartItem) => {
    try {
        const response = await fetch(`${BASE_URL}/api/basket`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(cartItem)
        });
        return response.ok;
    } catch (error) {
        console.error('Add error:', error);
        return false;
    }
};

const updateCartBackend = async (cartItem) => {
    try {
        const response = await fetch(`${BASE_URL}/api/basket`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(cartItem)
        });
        return response.ok;
    } catch (error) {
        console.error('Update error:', error);
        return false;
    }
};

const removeFromCartBackend = async (cartItem) => {
    try {
        const response = await fetch(`${BASE_URL}/api/basket`, {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(cartItem)
        });
        return response.ok;
    } catch (error) {
        console.error('Delete error:', error);
        return false;
    }
};

const checkout = async () => {
    if (!cart.items.length) {
        alert('Your cart is empty');
        return;
    }

    try {
        const response = await fetch(`${BASE_URL}/api/order/checkout`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(cart)
        });

        if (!response.ok) {
            const errorText = await response.text();
            alert('Checkout failed: ' + errorText);
        } else {
            alert('Checkout successful!');
            cart = { items: [] };
            updateCart();
        }
    } catch (error) {
        console.error('Checkout error:', error);
        alert('An error occurred during checkout.');
    }
};

// Initialize
initializeApp();
